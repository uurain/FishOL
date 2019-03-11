function Msg_ReqLogin(account)
 	local req = Protol.MsgDefine_pb.ReqLogin()
 	req.accountId = account
 	local reqBuff = req:SerializeToString()
    Network.SendMessage(Msg.Id.ReqLogin, reqBuff)
end

function Msg_ReqEnterRoom()
 	local req = Protol.MsgDefine_pb.ReqAckEnterRoom()
 	local reqBuff = req:SerializeToString()
    Network.SendMessage(Msg.Id.ReqAckEnterRoom, reqBuff)
end

function Msg_ReqLeaveRoom()
 	local req = Protol.MsgDefine_pb.ReqAckLeaveRoom()
 	local reqBuff = req:SerializeToString()
    Network.SendMessage(Msg.Id.ReqAckLeaveRoom, reqBuff)
end

function Msg_ReqBullet(uid, bulletType, sPos, tPos)
 	local req = Protol.MsgDefine_pb.ReqAckBullet()
 	req.uid = uid
 	req.bullet_type = bulletType
 	req.tpos = tpos
 	req.sPos = sPos
 	local reqBuff = req:SerializeToString()
    Network.SendMessage(Msg.Id.ReqAckBullet, reqBuff)
end

function Msg_ReqHitFish(uid, fishId)
 	local req = Protol.MsgDefine_pb.ReqAckHitFish()
 	req.uid = uid
 	req.fish_id = fishId
 	local reqBuff = req:SerializeToString()
    Network.SendMessage(Msg.Id.ReqAckHitFish, reqBuff)
end