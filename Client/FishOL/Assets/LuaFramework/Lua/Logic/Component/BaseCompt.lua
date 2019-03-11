local BaseCompt = class("BaseCompt")

function BaseCompt:ctor()
	self.onwer = nil
	self.rootWorldObj = nil
	self.isDispose = false
end

function BaseCompt:Init()
    self.rootWorldObj = GameObject.New("FishModel")
    GameObject.DontDestroyOnLoad(self.rootWorldObj)
end

function FishCompt:SetVisible(val)
	if self.rootWorldObj ~= nil then
    	self.rootWorldObj:SetActive(val)
	end
end

function BaseCompt:SetLocalScale(val)
	if self.rootWorldObj ~= nil then
    	self.rootWorldObj.transform.localScale = val
	end
end

function BaseCompt:SetLocalEulerAngles(val)
	if self.rootWorldObj ~= nil then
    	self.rootWorldObj.transform.localEulerAngles = val
	end
end

function BaseCompt:SetLocalPos(val)
	if self.rootWorldObj ~= nil then
    	self.rootWorldObj.transform.localPosition = val
	end
end

function BaseCompt:Dispose()

    if self.rootWorldObj ~= nil then
        self.rootWorldObj.transform:DetachChildren()
        GameObject.Destroy(self.rootWorldObj)
        self.rootWorldObj = nil
    end

    self.isDispose = true
end

return BaseCompt