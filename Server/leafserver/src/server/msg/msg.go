package msg

import (
	"fmt"
	"os"
	"reflect"
	"strings"

	"github.com/name5566/leaf/network/protobuf"
)

var Processor = protobuf.NewProcessor()

func init() {
	Processor.Register(&ReqLogin{})
	Processor.Register(&AckLogin{})
	Processor.Register(&ReqAckEnterRoom{})
	Processor.Register(&ReqAckLeaveRoom{})
	Processor.Register(&AckPlayerEntryList{})
	Processor.Register(&AckPlayerLeaveList{})
	Processor.Register(&AckPublicPropertyList{})
	Processor.Register(&ReqAckBullet{})
	Processor.Register(&AckFishOpt{})
	Processor.Register(&AckError{})
	Processor.Register(&ReqAckHitFish{})

	createLuaMsgConfig()
}

// 用于生成lua的协议id以及解析包文件
func createLuaMsgConfig() {
	var content string = "Msg = {\n"
	var idContent = "	Id = {\n"
	var funcContent = "	Func = {\n"
	Processor.Range(func(id uint16, t reflect.Type) {

		idStr := fmt.Sprint(id)
		msgStr := strings.Replace(t.String(), "*msg.", "", -1)
		//fmt.Println(t.String() + ":" + strings.Replace(t.String(), "*msg.", "", -1))
		idContent += "		" + msgStr + " = " + idStr + ",\n"

		funcContent += "		[" + idStr + "] = function (p)\n"
		funcContent += "		    local msg = Protol.MsgDefine_pb." + msgStr + "()\n"
		funcContent += "		    msg:ParseFromString(p)\n"
		funcContent += "		    return msg\n"
		funcContent += "		end,\n"
	})

	idContent += "	},\n"
	funcContent += "	},\n"
	content += idContent + funcContent + "}"

	f, err := os.Create("msg_define.lua")
	if err != nil {
		fmt.Println(err.Error())
	} else {
		_, err = f.Write([]byte(content))
	}
}
