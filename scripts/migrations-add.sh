#!/bin/sh

./dotnet-ef-install.sh

cd /api/DiagramEditor.Web.API

dotnet ef migrations add $1 \
    --namespace DiagramEditor.Infrastructure.Migrations \
    -o ../DiagramEditor.Infrastructure/Migrations \
    --project ../DiagramEditor.Infrastructure \
    --verbose
