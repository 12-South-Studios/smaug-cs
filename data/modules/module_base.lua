-- BASEMODULE.LUA
-- This is the Base Module for the MUD
-- Revised: 2013.11.09
-- Author: Jason Murdick
f = loadfile(LDataPath() .. "\\modules\\types.lua")();


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
				--errorLog("base.lua:executeScript() -> script (" .. script .. ") -> unknown error -> " .. debug.traceback());
				return false;		
			else
				--errorLog("base.lua:executeScript() -> script (" .. script .. ") -> " .. err .. " -> " .. debug.traceback());
				return false;
			end
		end
    else
		--errorLog('base.lua:executeScript() -> called to execute script ' .. script .. '.');
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

-- EOF
