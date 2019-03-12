local UIRoom = class(require("View.BasePanel"))

function UIRoom:Init()
	self.super:Init()

	self.view:GetChild("btnBack").onClick:Add(self.OnClickBack, self)
	self.goldPanel = self.view:GetChild("GoldPanel")

	self.rewardPanel = self.view:GetChild("rewardPanel")
	self.rewardPanel.visible = false

	self.view:GetChild("btnLeftGun").onClick:Add(function ()
		self:ChangeGun(true)
	end)
	self.view:GetChild("btnRightGun").onClick:Add(function ()
		self:ChangeGun(false)
	end)

	self.gunIcon = self.view:GetChild("gunIcon")
	self.firePanel = self.view:GetChild("firePanel")
	self.firePanel.onClick:Add(self.OnClickFire, self)

	self.gunTable = {}
	for i=1,4 do
		self.gunTable[i] = {}
		local item = self.view:GetChild("Gun"..i)
		self.gunTable[i].item = item
		self.gunTable[i].btnLeft = item:GetChild("btnLeftGun")
		self.gunTable[i].btnRight = item:GetChild("btnRightGun")
		self.gunTable[i].gunIcon = item:GetChild("gunIcon")
		self.gunTable[i].ctState = item:GetController("ctState")
		self.gunTable[i].ctArr = item:GetController("ctArr")

		if i > 2 then
			local arr = item:GetChild("title")
			arr.rotation = 180
		end

		self.gunTable[i].btnLeft.onClick:Add(function ()
			self:ChangeGun(i, true)
		end)
		self.gunTable[i].btnRight.onClick:Add(function ()
			self:ChangeGun(i, false)
		end)
	end
	self.mineGun = nil

	self.curGunIndex = 1	
	self.roomLogic = LogicManager.Get(LogicType.Room)
end

function UIRoom:Show()

	if self.updatePlayerHandler == nil then
		self.updatePlayerHandler = handler(self.UpdatePlayer, self)
		self.roomLogic:RegisterCallback("Action_Room_PlayerEnterLeave", self.updatePlayerHandler)
	end

	self.super.Show(self)
end

function UIRoom:Hide()
	self.mineGun = nil
	if self.updatePlayerHandler ~= nil then
		self.roomLogic:RemoveCallBackById("Action_Room_PlayerEnterLeave")
		self.updatePlayerHandler = nil
	end

	self.super.Hide(self)
end


function UIRoom:OnClickBack()

end

function UIRoom:OnClickFire()
	if self.mineGun ~= nil then
		local gunIconPos = self.mineGun.gunIcon:LocalToGlobal(Vector2.zero)
		local touchPos = Stage.inst.touchPosition
		touchPos:Sub(gunIconPos)
		local angle = Vector2.Angle(touchPos, Vector2.New(1, 0))
		log(angle)
		self.mineGun.gunIcon.rotation = 90 - angle

        -- local screenPos = self.btnOk:LocalToGlobal(Vector2.New(self.btnOk.width/2, self.btnOk.height/2))
        -- local screen3pos = Vector3.New(screenPos.x, screenPos.y, modeScale/1000 - gameSceneMgr.ui3dCam.transform.localPosition.z)
        -- local ui3dCamPos = gameSceneMgr.ui3dCam:ScreenToWorldPoint(screen3pos)
	end
end

function UIRoom:ChangeGun(index, isLeft)
	if isLeft then
		self.curGunIndex = self.curGunIndex -1
	else 
		self.curGunIndex =  self.curGunIndex+1
	end

	if self.curGunIndex < 1 then
		self.curGunIndex = 1
	end
	if self.curGunIndex > 3 then
		self.curGunIndex = 3
	end
	local gunIcon = self.gunTable[index].gunIcon
	gunIcon.url = UIPackage.GetItemAsset("Battle", "Gun_"..self.curGunIndex)
end

function UIRoom:UpdatePlayer(index, playerInfo)
	local gunItem = self.gunTable[index]

	gunItem.ctArr.selectedIndex = 1
	if playerInfo == nil then
		gunItem.ctState.selectedIndex = 1		
	else
		gunItem.ctState.selectedIndex = 0
		if playerInfo:IsMine() then
			self.mineGun = gunItem
			gunItem.ctArr.selectedIndex = 0
		end
		gunItem.item.title = playerInfo.player_info.name
	end
end

function UIRoom:InitPlayerTable()
	local playerTable = self.roomLogic.playerTable
	for k,v in pairs(self.gunTable) do
		self:UpdatePlayer(k, playerTable[k])
	end
end

function UIRoom:OnReward(gold)
	self.rewardPanel.visible = true
	self.rewardPanel.title = "获得金币:"..gold
	TimeMgr.DelayDo(function ()
		self.rewardPanel.visible = false
	end, 1)
end

return UIRoom