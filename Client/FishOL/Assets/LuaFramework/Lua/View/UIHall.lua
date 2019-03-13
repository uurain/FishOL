local UIHall = class(require("View.BasePanel"))


function UIHall:Init()
	self.super:Init()

	for i=1,3 do
		self.view:GetChild("btnRoom"..i).onClick:Add(self.OnClickEnterGame, self)
	end

	self.hallLogic = LogicManager.Get(LogicType.Hall)
	self.hallLogic:RegisterCallback("Action_Hall_EnterRoom", handler(self.OnEnterRoom, self))
end

function UIHall:OnClickEnterGame()
	self.hallLogic:ReqEnterRoom()
end

function UIHall:OnEnterRoom()
	UIManager.ShowPanel(UIPanelType.Room, function ()
		self:Hide()
	end)
end

return UIHall