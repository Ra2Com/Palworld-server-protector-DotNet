# Palworld-server-protector-DotNet

## 功能
- 内存监控（自定义阈值触发）
- 进程守护（当前如果没有服务端运行就自动重启）
- 优雅重启（内存占用达到阈值后自动发送公告并关服等待重启）
- 自动备份存档
- 轮询获取在线玩家
- Rcon指令
- 服务器配置文档(.ini)可视化编辑【New】
- Webhoot通知推送(企业微信机器人)【New】

## 注意
- 原库（[https://github.com/KirosHan/Palworld-server-protector-DotNet](https://github.com/KirosHan/Palworld-server-protector-DotNet)）


## 已知问题
1.受服务端限制，rcon发送的文本中无法保留空格，已自动替换为下划线

2.受服务端限制，rcon无法发送中文


