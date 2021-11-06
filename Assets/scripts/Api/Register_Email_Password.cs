using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// /RegisterClass
/// </summary>
public class UserRegister
{
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string gander { get; set; }
    public string address { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime created_at { get; set; }
    public int id { get; set; }
}

public class DataRegister
{
    public UserRegister user { get; set; }
    public string token { get; set; }
}

public class Register
{
    public int statsu { get; set; }
    public string message { get; set; }
    public DataRegister data { get; set; }
}












////////LoginClass



public class Headers
{
}

public class UserLogin
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string address { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public object email_verified_at { get; set; }
    public int gander { get; set; }
}

public class OriginalLogin
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public UserLogin user { get; set; }
}

public class DataLogin
{
    public Headers headers { get; set; }
    public OriginalLogin original { get; set; }
    public object exception { get; set; }
}

public class Login
{
    public int statsu { get; set; }
    public string message { get; set; }
    public DataLogin data { get; set; }
}

