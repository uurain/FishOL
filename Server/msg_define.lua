Msg = {
	Id = {
		ReqLogin = 0,
		AckLogin = 1,
		ReqAckEnterRoom = 2,
		ReqAckLeaveRoom = 3,
		AckPlayerEntryList = 4,
		AckPlayerLeaveList = 5,
		AckPublicPropertyList = 6,
		ReqAckBullet = 7,
		AckFishOpt = 8,
		AckError = 9,
		ReqAckHitFish = 10,
	},
	Func = {
		[0] = function (p)
		    local msg = Protol.MsgDefine_pb.ReqLogin()
		    msg:ParseFromString(p)
		    return msg
		end,
		[1] = function (p)
		    local msg = Protol.MsgDefine_pb.AckLogin()
		    msg:ParseFromString(p)
		    return msg
		end,
		[2] = function (p)
		    local msg = Protol.MsgDefine_pb.ReqAckEnterRoom()
		    msg:ParseFromString(p)
		    return msg
		end,
		[3] = function (p)
		    local msg = Protol.MsgDefine_pb.ReqAckLeaveRoom()
		    msg:ParseFromString(p)
		    return msg
		end,
		[4] = function (p)
		    local msg = Protol.MsgDefine_pb.AckPlayerEntryList()
		    msg:ParseFromString(p)
		    return msg
		end,
		[5] = function (p)
		    local msg = Protol.MsgDefine_pb.AckPlayerLeaveList()
		    msg:ParseFromString(p)
		    return msg
		end,
		[6] = function (p)
		    local msg = Protol.MsgDefine_pb.AckPublicPropertyList()
		    msg:ParseFromString(p)
		    return msg
		end,
		[7] = function (p)
		    local msg = Protol.MsgDefine_pb.ReqAckBullet()
		    msg:ParseFromString(p)
		    return msg
		end,
		[8] = function (p)
		    local msg = Protol.MsgDefine_pb.AckFishOpt()
		    msg:ParseFromString(p)
		    return msg
		end,
		[9] = function (p)
		    local msg = Protol.MsgDefine_pb.AckError()
		    msg:ParseFromString(p)
		    return msg
		end,
		[10] = function (p)
		    local msg = Protol.MsgDefine_pb.ReqAckHitFish()
		    msg:ParseFromString(p)
		    return msg
		end,
	},
}