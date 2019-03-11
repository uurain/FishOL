local HallLogic = class(require("Logic.BaseLogic"))

function HallLogic:Init()
	self.playTable = {}

	Event.AddListener(Msg.Id.ReqAckEnterRoom, handler(self, self.OnEnterRoom))
end

function HallLogic:OnEnterRoom(msg)
	self.super.DoEvent(self, "Action_Hall_EnterRoom")
end

function HallLogic:ReqEnterRoom()
	Msg_ReqEnterRoom()
end

return HallLogic