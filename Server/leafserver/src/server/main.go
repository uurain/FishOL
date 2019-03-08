package main

import (
	//	"fmt"
	"server/conf"
	"server/game"
	"server/gate"
	"server/login"
	//	"server/msg"

	//	"github.com/golang/protobuf/proto"
	"github.com/name5566/leaf"
	lconf "github.com/name5566/leaf/conf"
)

func main() {
	//	msg_test := &msg.Header{
	//		Cmd: proto.Int64(10),
	//		Seq: proto.Int32(20),
	//	}
	//	in_data, err := proto.Marshal(msg_test)
	//	if err != nil {
	//		fmt.Println("Marshaling error: ", err)
	//	}
	//	newTest := &msg.Header{}
	//	err = proto.Unmarshal(in_data, newTest)
	//	if err != nil {
	//		fmt.Println("Unmarshaling error: ", err)
	//	}
	//	fmt.Println(*newTest.Cmd)
	//	fmt.Println(*newTest.Seq)

	//	return
	lconf.LogLevel = conf.Server.LogLevel
	lconf.LogPath = conf.Server.LogPath
	lconf.LogFlag = conf.LogFlag
	lconf.ConsolePort = conf.Server.ConsolePort
	lconf.ProfilePath = conf.Server.ProfilePath

	leaf.Run(
		game.Module,
		gate.Module,
		login.Module,
	)
}
