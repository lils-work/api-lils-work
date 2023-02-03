FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY LilsWorkApi/LilsWorkApi/bin/Debug/net6.0/publish/ App/
WORKDIR /App
ENTRYPOINT [ "dotnet", "LilsWorkApi.dll" ]