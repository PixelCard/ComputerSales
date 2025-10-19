FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /ComputerProject
COPY ["ComputerSalesProject_MVC/ComputerSalesProject_MVC.csproj", "ComputerSalesProject_MVC/"]
COPY ["ComputerSales.Application/ComputerSales.Application.csproj","ComputerSales.Application/"]
COPY ["ComputerSales.Domain/ComputerSales.Domain.csproj","ComputerSales.Domain/"]
COPY ["ComputerSales.Infrastructure/ComputerSales.Infrastructure.csproj","ComputerSales.Infrastructure/"]


RUN dotnet restore "ComputerSalesProject_MVC/ComputerSalesProject_MVC.csproj"
COPY . .
WORKDIR "/ComputerProject/ComputerSalesProject_MVC"	
RUN dotnet build "ComputerSalesProject_MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComputerSalesProject_MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","ComputerSalesProject_MVC.dll"]


