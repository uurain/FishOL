local PropertySyncLogic = class(require("Logic.BaseLogic"))

function PropertySyncLogic:Init()
	Event.AddListener(Msg.Id.AckPublicPropertyList, handler(self.OnPublicPropertyList, self))
end

function PropertySyncLogic:OnPublicPropertyList(msg)
	if msg.uid == PlayerData.uid then
		PlayerData.gold = msg.player_info.gold
	end
end

return PropertySyncLogic