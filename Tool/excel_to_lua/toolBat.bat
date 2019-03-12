for %%i in (*.xlsx) do (
 .\\src %%i .\\
)

copy .\\*.lua  ..\\..\\Client\\FishOL\\Assets\\LuaFramework\\Lua\\Table\\ /Y

pause