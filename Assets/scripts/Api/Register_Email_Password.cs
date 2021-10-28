using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country
{
    public string name { get; set; }
    public string name_en { get; set; }
}

public class ClientRegister
{

    public string phone { get; set; }
    public string email { get; set; }
    public string phone_code { get; set; }
    public string name { get; set; }
    public string country_id { get; set; }
    public int activity { get; set; }
    public int gander { get; set; }
    public string address { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime created_at { get; set; }
    public int id { get; set; }
    public Country country { get; set; }
}

public class DataRegister
{
    public string api_token { get; set; }
    public ClientRegister client { get; set; }
}

public class Register
{
    public int state { get; set; }
    public string msg { get; set; }
    public DataRegister data { get; set; }
}



public class ClientLogin
{
    public int id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string name { get; set; }
    public int activity { get; set; }
    public string phone { get; set; }
    public int phone_code { get; set; }
    public int balance { get; set; }
    public string email { get; set; }
    public int country_id { get; set; }
    public int gander { get; set; }
    public string address { get; set; }
    public string img { get; set; }
    public Country country { get; set; }
}

public class DataLogin
{
    public string api_token { get; set; }
    public ClientLogin client { get; set; }
}

public class Login
{
    public int state { get; set; }
    public string msg { get; set; }
    public DataLogin data { get; set; }
}

