#!/bin/bash

sfctl application upload --path sfchat --show-progress
sfctl application provision --application-type-build-path sfchat
sfctl application create --app-name fabric:/sfchat --app-type sfchatType --app-version 1.0.0
