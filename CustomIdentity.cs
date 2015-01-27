
    public class CustomIdentity : GenericIdentity
    {
        private readonly Guid _id;

        private readonly bool _isPwdChangeRequired;

        public Guid Id
        {
            get { return _id; }
        }

        public bool IsPwdChangeRequired
        {
            get { return _isPwdChangeRequired; }
        }

        public CustomIdentity(string name, Guid id, bool isPwdChangeRequired)
            : base(name)
        {
            _id = id;
            _isPwdChangeRequired = isPwdChangeRequired;
        }
    }