using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Authentication myDeserializedClass = JsonConvert.DeserializeObject<Authentication>(myJsonResponse); 



public class Client
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
    public object gander { get; set; }
    public object address { get; set; }
    public string img { get; set; }
    public Country country { get; set; }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Country
{
    public string name { get; set; }
    public string name_en { get; set; }
}

public class ClientsignUp
{
    public string phone { get; set; }
    public string email { get; set; }
    public string phone_code { get; set; }
    public string name { get; set; }
    public string country_id { get; set; }
    public int activity { get; set; }
    public DateTime updated_at { get; set; }
    public DateTime created_at { get; set; }
    public int id { get; set; }
    public Country country { get; set; }
}


public class Datasignup
{
    public string api_token { get; set; }
    public ClientsignUp clients { get; set; }
}




public class Data
{
    public string api_token { get; set; }
    public Client client { get; set; }
}

public class AuthenticationAPi
{
    public int state { get; set; }
    public string msg { get; set; }
    public Data data { get; set; }
}
public class SignUpToApi
{
    public int state { get; set; }
    public string msg { get; set; }
    public Datasignup data { get; set; }
}
public class AuthenticationAPiFailed
{
    public int state { get; set; }
    public string msg { get; set; }
    public string data { get; set; }
}

public class AuthenticationAPiFailedsignup
{
    public int state { get; set; }
    public string msg { get; set; }
    public DataFailedSignup data { get; set; }
}
public class DataFailedSignup
{
    public string phone { get; set; }

    public string email { get; set; }


}

