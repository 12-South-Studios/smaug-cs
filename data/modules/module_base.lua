-- BASEMODULE.LUA
-- This is the Base Module for the MUD
-- Revised: 2013.11.09
-- Author: Jason Murdick
f = loadfile(GetAppSetting("dataPath") .. "\\types.lua")();


--==================================================================
 --FUNCTION: EXECUTE()
 --PARAM:    script / string
 --PARAM:    isfile / boolean
 --RETURN:   none
 --PURPOSE:  Executes the passed script or file
--==================================================================
function executeScript(script, isfile)
	
	if (isfile == true) then
        n = loadfile(script)
		
		local status, err = pcall(n)
		if status == false then
			if err == nil then
				errorLog("base.lua:executeScript() -> script (" .. script .. ") -> unknown error -> " .. debug.traceback());
				return false;		
			else
				errorLog("base.lua:executeScript() -> script (" .. script .. ") -> " .. err .. " -> " .. debug.traceback());
				return false;
			end
		end
    else
		errorLog('base.lua:executeScript() -> called to execute script ' .. script .. '.');
		return false;
    end

	return true;
end

function padStringToLeft( str, chr, wid )
    local n = wid - string.len( str )
    while( n > 0 ) do
        str = chr .. str
        n = n - 1
    end
    return str
end

function padStringToRight( str, chr, wid )
    local n = wid - string.len( str )
    while( n > 0 ) do
        str = str .. chr
        n = n - 1
    end
    return str
end

function setEnumGameProperty(enum, propertyValue)
	setGameProperty(enumToString(enum, true), propertyValue);
end

function setGameProperty(propertyName, propertyValue)
	setProperty(nil, propertyName, propertyValue);
end

function getGameProperty(propertyName)
	return getProperty(nil, propertyName);
end


--==================================================================
-- FUNCTION: SETLASTENTEREDGAME()
-- PARAM:    object
-- RETURN:   void
-- PURPOSE:  Sets the object that last entered the MUD
--==================================================================
function setLastEnteredGame(user)

	if (user == nil) then
		errorLog("base.lua:setLastEnteredGame -> user param was nil");
		return;
	end
	
	setProperty(nil, "last entered game", user);
end


--==================================================================
-- FUNCTION: GETLASTCONNECTED()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that most recently 
--           connected to the server.
--==================================================================
function getLastConnected()
	
	local oObj = getProperty(nil, 'last_connected');
    if (oObj == nil) then 
		errorLog('base.lua:getLastConnected -> getProperty returned nil');
        return;
    end;
    
    return oObj;
end


--==================================================================
-- FUNCTION: GETLASTDISCONNECTED()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that most recently 
--           disconnected from the server.
--==================================================================
function getLastDisconnected()
	
	local oObj = getProperty(nil, 'last disconnected')
    if (oObj == nil) then 
		errorLog('base.lua:getLastDisconnected -> getProperty returned nil') 
        return
    end
    
    return oObj
end


--==================================================================
-- FUNCTION: GETLASTENTEREDGAME()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that most recently 
--           successfully logged into the server.
--==================================================================
function getLastEnteredGame()
	
	local oObj = getProperty(nil, 'last entered game');
    if (oObj == nil) then 
		errorLog('base.lua:getLastEnteredGame -> getProperty returned nil') 
        return
    end
    
    return oObj
end


--==================================================================
-- FUNCTION: GETLASTCOMMANDVERB()
-- PARAM:    user / object
-- RETURN:   string
-- PURPOSE:  Returns the last verb (string) that the passed user
--           object submitted.
--==================================================================
function getLastCommandVerb(user)
	
	local sStr = getProperty(user, 'last command verb')
	if (sStr == nil) then
		errorLog('base.lua:getLastCommandVerb -> getProperty returned nil')
		return
	end

	return sStr
end


--==================================================================
-- FUNCTION: GETLASTCOMMANDSTRING()
-- PARAM:    user / object
-- RETURN:   string
-- PURPOSE:  Returns the last command (string) that the passed user
--           object submitted.
--==================================================================
function getLastCommandString(user)
	
	local sStr = getProperty(user, 'last command string')
	if (sStr == nil) then
		errorLog('base.lua:getLastCommandString -> getProperty returned nil')
		return
	end

	return sStr
end


--==================================================================
-- FUNCTION: GETCURRENTUSER()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that caused the current
--           event trigger to fire.
--==================================================================
function getCurrentUser()
    
    local oObj = getProperty(nil, 'current user')
    if (oObj == nil) then
		errorLog('base.lua:getCurrentUser -> getProperty returned nil')
		return
	end
	
	return oObj
end


--==================================================================
-- FUNCTION: GETLASTSPOKE()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that most recently 
--           spoke.
--==================================================================
function getLastSpoke()
	
	local oObj = getProperty(nil, 'last spoke')
    if (oObj == nil) then 
		errorLog('base.lua:getLastSpoke -> getProperty returned nil') 
        return
    end
    
    return oObj
end


--==================================================================
-- FUNCTION: GETLASTHEARD()
-- PARAM:    none
-- RETURN:   object
-- PURPOSE:  Returns a pointer to the object that most recently 
--           heard someone speak.
--==================================================================
function getLastHeard()
	
	local oObj = getProperty(nil, 'last heard')
    if (oObj == nil) then 
		errorLog('base.lua:getLastHeard -> getProperty returned nil') 
        return
    end
    
    return oObj
end
  
  
--==================================================================
-- FUNCTION: GETLASTVERB()
-- PARAM:    object
-- RETURN:   string
-- PURPOSE:  Returns a string that was the last verb issued by the
--           passed user object.
--==================================================================
function getLastVerb(user)
	
	local sVerb = getProperty(user, 'last command verb')
    if (sVerb == nil) then 
		errorLog('base.lua:getLastVerb -> getProperty returned nil') 
        return
    end
    playerLog(sVerb)
    return sVerb
end


--==================================================================
-- FUNCTION: GETLASTSPEAKSTRING()
-- PARAM:    object
-- RETURN:   string
-- PURPOSE:  Returns a string that was the last string spoken by 
--           the passed user object.
--==================================================================
function getLastSpeakString()
	
	local sCmd = getProperty(nil, 'last command string')
    if (sCmd == nil) then 
		errorLog('base.lua:getLastSpeakString -> getProperty returned nil') 
        return
    end
    
    return sCmd
end


--==================================================================
-- FUNCTION: GETLASTGREETED()
-- PARAM:    object
-- RETURN:   string
-- PURPOSE:  Returns a string that was the last string greeted by 
--           the passed user object.
--==================================================================
function getLastGreeted()
	
	local sCmd = getProperty(nil, 'last greeted')
    if (sCmd == nil) then 
		errorLog('base.lua:getLastGreeted -> getProperty returned nil') 
        return
    end
    
    return sCmd
end	
-- EOF
