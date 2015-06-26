#!/bin/bash

#Send sms to singtel bizlive sms aggregator

COMMAND=`curl https://www.bizlive.singtel.com/aggt/SendMsg.jsp?ID\=crmsystem100\&Password\=qrxy53tohh88\&Mobile\=\6592146688\&Type\=A\&Message\=$1`

echo "$COMMAND"

