protoc --go_out=. MsgDefine.proto

copy .\\*.go  ..\\..\\server\\leafserver\\src\\server\\msg\\ /Y

pause