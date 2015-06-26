#!/usr/bin/python

import urllib2

rcode = urllib2.urlopen('https://sslv3.dshield.org/vulnpoodle.png').read()
print rcode



