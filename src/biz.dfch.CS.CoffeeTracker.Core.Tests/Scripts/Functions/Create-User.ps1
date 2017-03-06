function Create-User{
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
		$uri = "CoffeeTracker/api/Users";

		if($Password.Length -lt 6)
		{
			throw "Password is to weak (at least 6 characters)";
		}

		$userBody = @{
			Name = $UserName
			Password = $Password
			IsAdmin = $false
		}

		try
		{
			$result = Invoke-RestMethod -Method Post -Uri $uri -Body $userBody
		}
		catch {
			throw "User could not be created.";
		}

		$OutputParameter = $result;
	}
	End
	{
		return $OutputParameter;
	}
}
#
# Copyright $year$ d-fens GmbH
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
