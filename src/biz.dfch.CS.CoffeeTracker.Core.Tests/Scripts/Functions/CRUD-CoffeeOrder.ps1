function CRUD-CoffeeOrder{
	PARAM
	(
		[Parameter(Mandatory = $true, Position = 0)]
		[ValidateNotNullOrEmpty()]
		[string] $Name
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[ValidateNotNullOrEmpty()]
		[string] $NewName
		,
		[ValidateRange(0, [long]::MaxValue)]
		[long] $CoffeeId
		,
		[ValidateRange(0, [long]::MaxValue)]
		[long] $UserId
		,
		[ValidateNotNull()]
		[DateTimeOffset] $Created
		,
		[Parameter(Mandatory = $true)]
		[ValidateNotNullOrEmpty()]
		[string] $Token
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Create')]
		[switch] $Create
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Update')]
		[switch] $Update
		,
		[Parameter(Mandatory = $false, ParameterSetName = 'Delete')]
		[switch] $Delete
	)
	Process
	{
		$uri = "CoffeeTracker/api/CoffeeOrders";

		if($Token)
		{
			$authString = "bearer {0}" -f $Token;
			$headers = [System.Collections.Generic.Dictionary[[String],[String]]]::New();
			$headers.Add("Authorization", $authString);
		}
			
		if($PSCmdlet.ParameterSetName -eq 'Create')
		{
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
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Update')
		{
			$filter = '$filter';
			$getUri = "{0}?$filter=Name eq '{1}'" -f $uri, $UserName;

			
			$userResult =	Invoke-RestMethod -Method Get -Uri $getUri -Headers $headers;
			$user = $userResult.value;

			if($NewUserName)
			{
				if($NewUserName.Length -lt 4)
				{
					throw "Invalid Name (at least 4 characters)"
				}

				$userBody = @{
					Name = $NewUserName
					Password = $user.Password
					IsAdmin = $user.IsAdmin
				}
				
			}
			elseif($password)
			{
				if($Password.Length -lt 6)
				{
					throw "Password is to weak (at least 6 characters)";
				}

				$userBody = @{
					Name = $user.Name
					Password = $Password
					IsAdmin = $user.IsAdmin
				}
			}

			$headers.Add("Content-Type", "application/json;odata=verbose")
			$updateUri = "$uri({0})" -f $user.Id;

			$userBodyJson = $userBody | ConvertTo-Json;

			$result = Invoke-RestMethod -Method Put -Uri $updateUri -Body $userBodyJson -Headers $headers;
		}
		elseif($PSCmdlet.ParameterSetName -eq 'Delete')
		{
			. .\Delete-Entities.ps1;
			$result = Delete-Entities -EntityName "Users" -ODataComparison "Name eq '$UserName'";
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
