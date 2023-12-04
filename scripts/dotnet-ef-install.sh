#!/bin/sh

dotnet tool install --global dotnet-ef > /dev/null
export PATH="$PATH:/root/.dotnet/tools"
