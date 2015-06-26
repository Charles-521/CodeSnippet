#!/bin/bash

MonitorServerName = 'xxxxxx'

COMMAND=`zabbix_sender -z 127.0.0.1 -p 10051 -s MonitorServerName -k "sms-trap-log" -o $1`

echo $COMMAND
