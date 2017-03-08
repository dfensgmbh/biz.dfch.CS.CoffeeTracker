function Get-Token {
	PARAM
	(
		[Parameter(Mandatory = $true, Position = 0)]
		[ValidateNotNullOrEmpty()]
		[string] $UserName
		,
		[Parameter(Mandatory = $true, Position = 0)]
		[ValidateNotNullOrEmpty()]
		[string] $Password
	)
	Process
	{
		$uri = "CoffeeTracker/token";

		$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
		$headers.Add("Content-Type","application/json");
		$headers.Add("Accept", "application/json");

		$body = @{
			grant_type = "password"
			username = $UserName
			password = $Password
		}

		try 
		{
			$result = Invoke-RestMethod -Method Post -Uri $uri -Headers $headers -Body $body;
			$OutputParameter = $result.access_token;
		}
		catch
		{
			$OutputParameter = "Invalid Username/Password combination";
		}
		
	}
	End
	{
		return $OutputParameter;
	}
}
#
# Copyright 2017 d-fens GmbH
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
