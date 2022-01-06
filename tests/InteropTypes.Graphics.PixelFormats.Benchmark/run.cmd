rem https://benchmarkdotnet.org/articles/guides/console-args.html#runtimes
rem https://benchmarkdotnet.org/articles/configs/toolchains.html

set DOTNET_CLI_UI_LANGUAGE=en

dotnet run -c Release -f net6.0 -- --runtimes Clr Mono net472 --job short
pause