#!/bin/bash

sudo systemctl stop LinkLookup

git pull
rm -rf /srv/LinkLookup
mkdir /srv/LinkLookup
chown root /srv/LinkLookup
dotnet publish -c Release -o /srv/LinkLookup

sudo cp LinkLookup.service /etc/systemd/system/LinkLookup.service
sudo systemctl daemon-reload
echo "run sudo systemctl start LinkLookup to start service"