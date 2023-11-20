FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS api-build
WORKDIR /app/api

RUN mkdir -p /app/web/src/api

COPY api/api.csproj .
RUN dotnet restore

COPY api/ .
ENV DOTNET_ROLL_FORWARD=LatestMajor
# TODO: publish
RUN dotnet tool restore && dotnet build

FROM node:18-alpine AS web-build
WORKDIR /app/web

COPY web/package.json web/package-lock.json web/.npmrc ./
RUN npm install

COPY --from=api-build /app/api/swagger.json /app/web/src/api/swagger.json
COPY web/ .
RUN npm run codegen

