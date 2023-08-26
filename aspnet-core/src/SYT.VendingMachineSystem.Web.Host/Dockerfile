FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["src/SYT.VendingMachineSystem.Web.Host/SYT.VendingMachineSystem.Web.Host.csproj", "src/SYT.VendingMachineSystem.Web.Host/"]
COPY ["src/SYT.VendingMachineSystem.Web.Core/SYT.VendingMachineSystem.Web.Core.csproj", "src/SYT.VendingMachineSystem.Web.Core/"]
COPY ["src/SYT.VendingMachineSystem.Application/SYT.VendingMachineSystem.Application.csproj", "src/SYT.VendingMachineSystem.Application/"]
COPY ["src/SYT.VendingMachineSystem.Core/SYT.VendingMachineSystem.Core.csproj", "src/SYT.VendingMachineSystem.Core/"]
COPY ["src/SYT.VendingMachineSystem.EntityFrameworkCore/SYT.VendingMachineSystem.EntityFrameworkCore.csproj", "src/SYT.VendingMachineSystem.EntityFrameworkCore/"]
WORKDIR "/src/src/SYT.VendingMachineSystem.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/SYT.VendingMachineSystem.Web.Host", "src/SYT.VendingMachineSystem.Web.Host"]
COPY ["src/SYT.VendingMachineSystem.Web.Core", "src/SYT.VendingMachineSystem.Web.Core"]
COPY ["src/SYT.VendingMachineSystem.Application", "src/SYT.VendingMachineSystem.Application"]
COPY ["src/SYT.VendingMachineSystem.Core", "src/SYT.VendingMachineSystem.Core"]
COPY ["src/SYT.VendingMachineSystem.EntityFrameworkCore", "src/SYT.VendingMachineSystem.EntityFrameworkCore"]
WORKDIR "/src/src/SYT.VendingMachineSystem.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "SYT.VendingMachineSystem.Web.Host.dll"]