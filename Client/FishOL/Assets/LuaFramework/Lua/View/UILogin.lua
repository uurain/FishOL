require "Common/define"
local UILogin = class(require("View.BasePanel"))

function UILogin:ctor()
	log("UILogin:ctor")
	-- self.super.ctor(self,UIPanelType.MainGame)
	-- self.btnBegin = nil
	self.btnLogin = nil
	self.tfUsername = nil
	self.tfPassword = nil
	self.groupLogin = nil
	self.serverList = nil
end

--初始化面板--
function UILogin:Init()
	log("UILogin:init")
	self.super.Init(self)

	self.btnLogin = self.view:GetChild("btnLogin")
	self.btnLogin.onClick:Add(self.OnClickLogin, self)


	self.tfUsername = self.view:GetChild("inputUsername")

	local localName = self:GetUserName()
	if localName ~= "" then
		self.tfUsername.text = localName
	end
	
	self.loginLogic = LogicManager.Get(UIPanelType.Login.name)
	self.loginLogic:RegisterCallback("LOGINSUCESS", handler(self.OnLoginSucess, self))
end


function UILogin:OnClickLogin()
	log("onClickLogin")
	-- self:Hide()
	-- LogicManager.Get(LogicType.GameMgr):LocalTest()
	self.loginLogic:ReqLogin(self.tfUsername.text)
end

function UILogin:OnLoginSucess()
	UIManager.ShowPanel(UIPanelType.Hall, function ()
		self:Hide()
	end)
end

function UILogin:GetUserName()
	local localName =  self.tfUsername.text
	if localName ~= '' then 
		G_SaveDataMgr:setString("TempLogin", self.tfUsername.text)
		return localName
	end
	localName = G_SaveDataMgr:getString("TempLogin", '')
	if localName ~= '' then
		return localName
	end
	localName = "Guest:"..os.time()
	G_SaveDataMgr:setString("TempLogin", localName)
	return localName
end

return UILogin