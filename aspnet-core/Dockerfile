<<<<<<< Updated upstream

=======
# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 8082


# 此阶段用于生成服务项目
ARG BUILD_CONFIGURATION=Release
# 将发布后的文件复制到容器中（注意路径）
COPY ./publish/ .

# 指定启动入口（替换为你的主程序 DLL 名）
ENTRYPOINT ["dotnet", "CustomerRelationshipManagement.HttpApi.Host.dll"]
>>>>>>> Stashed changes
