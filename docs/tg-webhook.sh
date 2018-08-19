#! /bin/bash

echo "Attempting CURL tg-webhook"
sleep 30
curl http://127.0.0.1:5051/api/bot/telegram
