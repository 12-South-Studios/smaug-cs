-- BASEMODULE.LUA
-- This is the Base Module for the MUD
-- Revised: 2013.11.09
-- Author: Jason Murdick

-- Does the table contain the value
function SetContains(set, key)
	return set[key] ~= nil
end

function table.contains(table, element)
  for _, value in pairs(table) do
	if value == element then
	  return true
	end
  end
  return false
end

--==================================================================
 --FUNCTION: EXECUTE()
 --PARAM:    script / string
 --PARAM:    isfile / boolean
 --RETURN:   none
 --PURPOSE:  Executes the passed script or file
--==================================================================
function executeScript(script, isfile)
	if (isfile == false) then
        LBootLog('ExecuteScript -> called to execute string ' .. script .. '.');
		return false;
    end

	local status, err = pcall(loadfile(script));
	if status == false then
		if err == nil then
			LBootLog("ExecuteScript -> script (" .. script .. ") -> unknown error -> " .. debug.traceback());
		else
			LBootLog("ExecuteScript -> script (" .. script .. ") -> " .. err .. " -> " .. debug.traceback());
		end
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
