#!/bin/bash

sfctl application delete --application-id sfchat
sfctl application unprovision --application-type-name sfchatType --application-type-version 1.0.0
sfctl store delete --content-path sfchat
