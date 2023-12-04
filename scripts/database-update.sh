#!/bin/sh

./dotnet-ef-install.sh

cd /api/DiagramEditor.Web.API

dotnet ef database update \
    --project ../DiagramEditor.Infrastructure \
    --verbose
