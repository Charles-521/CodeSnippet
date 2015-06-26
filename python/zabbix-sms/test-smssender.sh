#!/bin/bash

#Send sms to singtel bizlive sms aggregator

COMMAND=`./smssender "{\"AlertMessage\":\"Testing SMS Sender\"}" `

echo "$COMMAND"

