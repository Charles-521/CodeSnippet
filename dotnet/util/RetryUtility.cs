	public class RetryUtility
    {
        private readonly int retryCount = 3;
        private readonly int _delayTillNextRetry = 2000;
        private readonly AlertLogger _logger;

        public RetryUtility(AlertLogger logger)
        {
            _logger = logger;
        }

        public T Retry<T>(Func<T> method)
        {
            int currentRetry = 0;

            T result;

            while (true)
            {
                try
                {
                    result = method.Invoke();
                    break;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    var args = new AlertLogArgs(string.Empty, string.Empty,
                        LogEnumDef.EventID.UnhandledException,
                        LogEnumDef.Category.UnhandledException,
                        Guid.Empty,
                        string.Format("{0} times retry attempt failed on {1}: {2}", currentRetry, method.Method.Name, ex.Message)
                        );
                    _logger.Warn(args);

                    if (currentRetry >= retryCount || IsTransient(ex))
                    {
                        throw;
                    }

                    Thread.Sleep(_delayTillNextRetry);
                }
            }
            return result;
        }

        private bool IsTransient(Exception ex)
        {
            if (ex is WebException)
                return false;
            return true;
        }

        public void Retry(Action method)
        {
            int currentRetry = 0;

            while (true)
            {
                try
                {
                    method.Invoke();
                    break;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    if (currentRetry > retryCount)
                    {
                        throw;
                    }
                    var args = new AlertLogArgs(string.Empty, string.Empty,
                        LogEnumDef.EventID.UnhandledException,
                        LogEnumDef.Category.UnhandledException,
                        Guid.Empty,
                        string.Format("{0} retry failed: {1}", retryCount, ex.Message)
                        );
                    _logger.Warn(args);

                    Thread.Sleep(_delayTillNextRetry);
                }
            }
        }
    }