local FishCompt = class(require("Logic.Component.BaseCompt"))

function FishCompt:ctor()
	log("FishCompt:ctor")
	self.assetNode = nil
end

function FishCompt:Init(fishDb)
	self.super.Init(self)
	
    self.assetNode = require("Logic.Component.GameObjectNode").new(fishDb.path)
    self.assetNode:Load(function (assetObj )
        assetObj.CachedTransform:SetParent(self.rootWorldObj.transform)
        assetObj.CachedTransform.localPosition = Vector3.zero
        assetObj.CachedTransform.localEulerAngles = Vector3.zero
        self.rootWorldObj.name = assetObj.CachedGameObject.name

        self.animator = assetObj.CachedGameObject:GetComponent("Animator")
        self.triggerScript = assetObj.CachedGameObject:GetComponent("FishTrigger")
        self.triggerScript.fishId = self.fishId
    end)

    self.fishDb = fishDb
    self.fishId = 0
end

function FishCompt:Begin(pathId, fishId)
	self:SetVisible(true)
	self.fishId = fishId
    self.pathTable = PathMgr.GetPath(pathId)
    self.pathCount = #self.pathTable
    self.pathIndex = 1
    self:SetLocalPos(self.pathTable[1])
    self.isTargetPos = false
    self.isMoveTrueDir = true 

    self:SetNextMove()
end

function FishCompt:OnUpdate(deltaTime)
	self:OnMove(deltaTime)
	self.super.OnUpdate(self, deltaTime)	
end

function FishCompt:PlayAni(aniName )
	if self.animator ~= nil then
		self.animator:Play(aniName)
	end
end

function FishCompt:OnMove(deltaTime)
	if self.isTargetPos then
		self.rootWorldObj.transform:Translate(Vector3.forward * self.fishDb.speed * deltaTime)
		local curPos = self.rootWorldObj.transform.position
		curPos.z = 0
		self.rootWorldObj.transform.position = curPos
		if Vector3.Distance(curPos, self.targetPos) < 0.1 then
			self.isTargetPos = false
			self:SetNextMove()
		end

		if curPos.x < -100 or curPos.x > 100 or curPos.y > 100 or curPos.y < -100 then
			self.isTargetPos = false
			self:SetNextMove()
		end
	end
end

function FishCompt:SetMoveTarget(tPos)
	self.rootWorldObj.transform:LookAt(tPos)
	self.targetPos = tPos
	self.isTargetPos = true
end

function FishCompt:SetNextMove()
	if self.isMoveTrueDir then
		self.pathIndex = self.pathIndex + 1
	else
		self.pathIndex = self.pathIndex - 1
	end
	if self.pathIndex >= self.pathCount then
		self.isMoveTrueDir = false
	end
	if self.pathIndex <= 1 then
		self.isMoveTrueDir = true
	end
	self:SetMoveTarget(self.pathTable[self.pathIndex])
end

function FishCompt:Dispose()
	if self.assetNode ~= nil then
		self.assetNode:Dipose()
		self.assetNode = nil
	end
	self.super.Dipose(self)
end

return FishCompt