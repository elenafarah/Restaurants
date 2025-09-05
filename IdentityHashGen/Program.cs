using System;
using Microsoft.AspNetCore.Identity;

var password = args.Length > 0 ? args[0] : "P@ssw0rd!";
var hasher = new PasswordHasher<IdentityUser>();
var user = new IdentityUser("admin@test.com");

var hash = hasher.HashPassword(user, password);
Console.WriteLine(hash);
