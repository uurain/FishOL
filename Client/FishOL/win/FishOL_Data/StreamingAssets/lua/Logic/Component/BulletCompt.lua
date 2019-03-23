local BulletCompt = class(require("Logic.Component.BaseCompt"))

function BulletCompt:ctor()
	self.assetNode = nil
end

function BulletCompt:Init(bulletDb)
	self.super.Init(self)

    self.assetNode = require("Logic.Component.GameObjectNode").new(bulletDb.path)
    self.assetNode:Load(function (assetObj )
        assetObj.CachedTransform:SetParent(self.rootWorldObj.transform)
        assetObj.CachedTransform.localPosition = Vector3.zero
        assetObj.CachedTransform.localEulerAngles = Vector3.zero

        self.rootWorldObj.name = assetObj.CachedGameObject.name

        self.triggerScript = assetObj.CachedGameObject:GetComponent("BulletTrigger")
        self.triggerScript.luaFunc = handler(self.OnTrigger, self)
    end)

    self.bulletDb = bulletDb
end

function BulletCompt:Begin(uid, sPos, tPos)
	self:SetLocalPos(sPos)
	self.uid = uid
    self.isTargetPos = false
    self.isEnd = false
    self:SetVisible(true)
    self:SetMoveTarget(tPos)


end

function BulletCompt:OnUpdate(deltaTime)
	if self.isEnd then
		return
	end
	self.super.OnUpdate(self, deltaTime)
	self:OnMove(deltaTime)
end

function BulletCompt:OnMove(deltaTime)
	if self.isTargetPos then
		self.rootWorldObj.transform:Translate(Vector3.forward * self.bulletDb.speed * deltaTime)
		local curPos = self.rootWorldObj.transform.position
		curPos.z = 0
		self.rootWorldObj.transform.position = curPos
		if Vector3.Distance(curPos, self.targetPos) < 0.1 or curPos.x < -100 or curPos.x > 100 or curPos.y > 100 or curPos.y < -100  then
			self:OnMoveEnd()
		end
	end
end

function BulletCompt:SetMoveTarget(tPos)
	self.rootWorldObj.transform:LookAt(tPos)
	self.targetPos = tPos
	self.isTargetPos = true
end

function BulletCompt:OnTrigger(fishId)
	logError("OnTrigger:"..fishId)
	self:OnMoveEnd()
	LogicManager.Get(LogicType.SceneMove):OnFishTrigger(fishId, self.uid)
end

function BulletCompt:OnMoveEnd()
	self.isTargetPos = false
	self.isEnd = true
	self:SetVisible(false)
end

function BulletCompt:Dispose()
	if self.assetNode ~= nil then
		self.assetNode:Dipose()
		self.assetNode = nil
	end
	self.super.Dipose(self)
end

return BulletCompt