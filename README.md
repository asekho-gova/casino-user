1.The subsequent folder structure should be:
  - `Casino-User-Api`
    - `src` folder - Containing a .NET 8 `Casino.User.Api` project
    - `.gitignore`
    - `Casino-User-Api.sln`
    - `README.md` (containing these instructions).

## Database
- This API uses a Sqlite database in the form of a `casinoUsers.db` file.
- This is purely for the purpose of making this assessment self-contained and should be treated as if it's a normal SQL server database.

- The database consists of 2 tables:
  - **tb_Users**
    ```sql
    CREATE TABLE "tb_Users" (
      "UserId"	INTEGER NOT NULL,
      "Username"	TEXT NOT NULL UNIQUE,
      "Password"	TEXT NOT NULL,
      "Email"	TEXT NOT NULL,
      "HomePhoneNumber"	TEXT,
      "WorkPhoneNumber"	TEXT,
      "MobilePhoneNumber"	TEXT,
      "Balance"	REAL NOT NULL DEFAULT 0,
      PRIMARY KEY("UserId")
    );
    ```
    Comes preloaded with two users with UserIds 1 and 2.

  - **tb_BalanceUpdateLog**
    ```sql
    CREATE TABLE "tb_BalanceUpdateLog" (
      "Id"	INTEGER NOT NULL,
      "UserId"	INTEGER NOT NULL,
      "UpdateAmount"	REAL NOT NULL,
      "Balance"	REAL NOT NULL,
      PRIMARY KEY("Id")
    );
    ```