local UILoading = class(require("View.BasePanel"))

function UILoading:ctor()	
	self.slider = nil
	self.loadingType = UILoadingType.battle
	self.beginLoading = false
end

function UILoading:Init()
	self.super:Init()
	self.slider = self.view:GetChild("pb")
	self:SetPercent(0)
	self.showBattle = false
	self.UpdateHandle = handler(self, self.Update)
end

function UILoading:Show()
	self.super.Show(self)

    self:SetPercent(0)
	self.showBattle = false
	UpdateBeat:Add(self.UpdateHandle)
end

function UILoading:SetLoadingType(type)
	self.loadingType = type
	if self.loadingType == UILoadingType.LuaLoad then
		self.beginLoading = false
	end

	logError("type:"..type)
end

function UILoading:BeginLoading()
	logError("beginLoading")
	self.beginLoading = true
end

function UILoading:Hide()
	self.super.Hide(self)
	UpdateBeat:Remove(self.UpdateHandle)
	self:SetPercent(0)
end

function UILoading:Update()

end

function UILoading:SetPercent( val)
	if self.slider ~= nil then
		self.slider.value = val * 100
	end

	-- log(val)

	if val > 0.999 then		
		if self.showBattle == false then
			self.showBattle = true
			self:hide()
		end
	end
end


return UILoading