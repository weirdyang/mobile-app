param(
    $PackagesDir,
    $OutFilePath
)

$ErrorActionPreference = "Stop"

Function Find-SplitIndex([string]$Name)
{
    $index = $Name.Length;
    while($true) {
        $newIndex = $Name.LastIndexOf('.', $index - 1)
        $substr = $Name.Substring($newIndex + 1, $index - $newIndex - 1)

        [int]$dummy = 0
        if([System.Int32]::TryParse($substr, [ref]$dummy))
        {
            $index = $newIndex
        }
        else
        {
            break
        }
    }

    return $index;
}

Function Parse-PackageName([string]$Name)
{
    $splitIndex = Find-SplitIndex $Name

    $result = New-Object -TypeName PSObject
    $result = $result | Add-Member -MemberType Property -Name PackageName -Value $Name.Substring(0, $splitIndex)
    $result = $result | Add-Member -MemberType Property -Name Version -Value $Name.Substring($splitIndex + 1)

    return $result
}

Get-ChildItem -Path $PackagesDir | `
    Select-Object @{name='name';expression={$_.Name.Substring(0, $(Find-SplitIndex $_.Name))}},`
                  @{name='version';expression={$_.Name.Substring($(Find-SplitIndex $_.Name) + 1)}} | `
    ConvertTo-Json | Out-File $OutFilePath