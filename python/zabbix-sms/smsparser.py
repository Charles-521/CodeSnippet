#retrieve alert message from event log data entry
import json

def parseMsg(m):
    #print 'original string: ' + m
    #nm = m.replace(' ', '+')
    #print 'After replace blankspace: ' + nm
    value = json.loads(m.replace(' ', '+'))
    return value['AlertMessage']
