local UIRoom = class(require("View.BasePanel"))

function UIRoom:Init()
	self.super:Init()

	self.view:GetChild("btnBack").onClick:Add(self.OnClickBack, self)
	self.goldPanel = self.view:GetChild("GoldPanel")

	self.rewardPanel = self.view:GetChild("rewardPanel")
	self.rewardPanel.visible = false

	self.firePanel = self.view:GetChild("firePanel")
	self.firePanel.onClick:Add(self.OnClickFire, self)

	self.gunTable = {}
	for i=1,4 do
		self.gunTable[i] = {}
		local item = self.view:GetChild("Gun"..i)
		self.gunTable[i].index = i
		self.gunTable[i].item = item
		self.gunTable[i].btnLeft = item:GetChild("btnLeftGun")
		self.gunTable[i].btnRight = item:GetChild("btnRightGun")
		self.gunTable[i].gun = item:GetChild("gun")
		self.gunTable[i].gunIcon = self.gunTable[i].gun:GetChild("icon")
		self.gunTable[i].ctState = item:GetController("ctState")
		self.gunTable[i].ctArr = item:GetController("ctArr")
		self.gunTable[i].gunPt = self.gunTable[i].gun:GetChild("gunPt")

		self.gunTable[i].ctState.selectedIndex = 1
		self.gunTable[i].ctArr.selectedIndex = 1

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
	self.localPlayer = nil

	self.roomLogic:RegisterCallback("Action_Room_HitFish", handler(self.OnHitFish, self))
end

function UIRoom:Show()

	if self.updatePlayerHandler == nil then
		self.updatePlayerHandler = handler(self.UpdatePlayer, self)
		self.roomLogic:RegisterCallback("Action_Room_PlayerEnterLeave", self.updatePlayerHandler)
	end

	self:InitPlayerTable()
	self:UpdateGold()

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
		local gunPos = self.mineGun.gun:LocalToGlobal(Vector2.zero)
		local touchPos = Stage.inst.touchPosition

		touchPos:Sub(gunPos)
		local angle = Vector2.Angle(touchPos, Vector2.New(1, 0))

		if self.mineGun.index > 2 then
			self.mineGun.gun.rotation = angle + 270
		else
			self.mineGun.gun.rotation = 90 - angle
		end		

		if self.roomLogic:CanFire(self.curGunIndex) then
			local touchTpos = Stage.inst.touchPosition
			local gunPtPos = self.mineGun.gunPt:LocalToGlobal(Vector2.zero)
			gunPtPos.y = Screen.height - gunPtPos.y
			touchTpos.y = Screen.height - touchTpos.y

			local sPos = MainCam:ScreenToWorldPoint(Vector3(gunPtPos.x, gunPtPos.y, 20))

			local tempPos = MainCam:ScreenToWorldPoint(Vector3(touchTpos.x, touchTpos.y, 20))
			local tPos = Vector3.New(tempPos.x, tempPos.y, tempPos.z)
			tPos:Add(tempPos:Sub(sPos).normalized:Mul(50))
			self.roomLogic:ReqFire(self.curGunIndex, sPos, tPos)

			self:UpdateGold()
		else
			logError("金币不够")
		end
        -- local screenPos = self.btnOk:LocalToGlobal(Vector2.New(self.btnOk.width/2, self.btnOk.height/2))
        -- local screen3pos = Vector3.New(screenPos.x, screenPos.y, modeScale/1000 - gameSceneMgr.ui3dCam.transform.localPosition.z)
        -- local ui3dCamPos = gameSceneMgr.ui3dCam:ScreenToWorldPoint(screen3pos)
	end
end

function UIRoom:OnHitFish(rewardGold)
	UIManager.ShowPanel(UIPanelType.Tip, function (ui)
		ui:ShowTip("获得金币:"..rewardGold)
	end)
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
	gunIcon.url = UIPackage.GetItemURL("Room", "Gun_"..self.curGunIndex)
end

function UIRoom:UpdatePlayer(index, player)

	log("UpdatePlayer:"..index)
	local gunItem = self.gunTable[index+1]

	gunItem.ctArr.selectedIndex = 1
	if player == nil then
		gunItem.ctState.selectedIndex = 1		
	else
		gunItem.ctState.selectedIndex = 1
		if player:IsMine() then
			self.localPlayer = player
			self.mineGun = gunItem
			gunItem.ctArr.selectedIndex = 0
		end
		gunItem.item.title = player:GetName()
	end
end

function UIRoom:UpdateGold()
	self.goldPanel.title = PlayerData.gold
end

function UIRoom:InitPlayerTable()
	local playerTable = self.roomLogic.playerTable
	for k,v in pairs(playerTable) do
		self:UpdatePlayer(k, v)
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