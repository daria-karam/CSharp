using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Lab7
{
    public class MyData
    {
        OleDbConnection cn = new OleDbConnection(
        @"Provider=Microsoft.ACE.OLEDB.12.0;" +
        @"Data Source=Lab7_DB.accdb;" +
        @"Jet OLEDB:Create System Database=true;" +
        @"Jet OLEDB:System database=C:\Users\Daria\AppData\Roaming\Microsoft\Access\System.mdw"
        );

        public List<Team> teams;
        public List<Contest> contests;
        public List<Result> results;
        public List<Sport> sports;
        public List<Sportsman> sportsmans;
        public List<Stadium> stadiums;
        public DataTable MSysObjectsData;

        public MyData()
        {
            teams = new List<Team>();
            contests = new List<Contest>();
            results = new List<Result>();
            sports = new List<Sport>();
            sportsmans = new List<Sportsman>();
            stadiums = new List<Stadium>();
            MSysObjectsData = new DataTable();
        }

        /************************ Search by ID ************************/

        public Team GetTeamFromDB(int ID)
        {
            Team team = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Команда WHERE [Идентификатор команды] = @TeamID";
                cmd.Parameters.AddWithValue("@TeamID", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        team = new Team(rd.GetInt32(0), rd.GetString(1), !rd.IsDBNull(2) ? rd.GetString(2) : "",
                            rd.GetInt32(3), !rd.IsDBNull(4) ? rd.GetString(4) : "");
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return team;
        }

        public Contest GetContestFromDB(int ID)
        {
            Contest contest = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Соревнование WHERE [Идентификатор соревнования] = @ContestID";
                cmd.Parameters.AddWithValue("@ContestID", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        contest = new Contest(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetValue(4).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return contest;
        }

        public Result GetResultFromDB(int ID)
        {
            Result result = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Результат WHERE [Номер попытки] = @AttemptNumber";
                cmd.Parameters.AddWithValue("@AttemptNumber", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        result = new Result(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetInt32(4));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return result;
        }

        public Sport GetSportFromDB(int ID)
        {
            Sport sport = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM [Вид спорта] WHERE [Идентификатор вида спорта] = @SportID";
                cmd.Parameters.AddWithValue("@SportID", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        sport = new Sport(rd.GetInt32(0), rd.GetString(1));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return sport;
        }

        public Sportsman GetSportsmanFromDB(int ID)
        {
            Sportsman sportsman = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Спортсмен WHERE [Номер спортсмена] = @SportsmanNumber";
                cmd.Parameters.AddWithValue("@SportsmanNumber", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        sportsman = new Sportsman(rd.GetInt32(0), rd.GetInt32(1),
                            !rd.IsDBNull(2) ? rd.GetString(2) : "", rd.GetString(3),
                            !rd.IsDBNull(4) ? rd.GetString(4) : "");
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return sportsman;
        }

        public Stadium GetStadiumFromDB(int ID)
        {
            Stadium stadium = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Стадион WHERE [Идентификатор стадиона] = @StadiumID";
                cmd.Parameters.AddWithValue("@StadiumID", ID);
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        stadium = new Stadium(rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetInt32(3));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
            return stadium;
        }

        /************************** Get data **************************/
        public void GetDataFromDB()
        {
            GetTeamsFromDB();
            GetContestsFromDB();
            GetResultsFromDB();
            GetSportsFromDB();
            GetSportsmansFromDB();
            GetStadiumsFromDB();
        }

        public void GetTeamsFromDB()
        {
            teams.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Команда";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        teams.Add(new Team(rd.GetInt32(0), rd.GetString(1), !rd.IsDBNull(2) ? rd.GetString(2) : "",
                            rd.GetInt32(3), !rd.IsDBNull(4) ? rd.GetString(4) : ""));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public void GetContestsFromDB()
        {
            contests.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Соревнование";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        contests.Add(new Contest(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetValue(4).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public void GetResultsFromDB()
        {
            results.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Результат";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        results.Add(new Result(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetInt32(4)));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public void GetSportsFromDB()
        {
            sports.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM [Вид спорта]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        sports.Add(new Sport(rd.GetInt32(0), rd.GetString(1)));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public void GetSportsmansFromDB()
        {
            sportsmans.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Спортсмен";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        sportsmans.Add(new Sportsman(rd.GetInt32(0), rd.GetInt32(1),
                            !rd.IsDBNull(2) ? rd.GetString(2) : "", rd.GetString(3),
                            !rd.IsDBNull(4) ? rd.GetString(4) : ""));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        public void GetStadiumsFromDB()
        {
            stadiums.Clear();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM Стадион";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        stadiums.Add(new Stadium(rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetInt32(3)));
                    }
                }
            }
            finally
            {
                cn.Close();
            }
        }

        /*************************** Delete ***************************/
        public string DeleteFromTableByID(int ID)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;

                switch (MainWindow.currentTableName)
                {
                    case "Соревнование":
                        cmd.CommandText = "DELETE FROM Соревнование WHERE [Идентификатор соревнования] = @Temp1";
                        break;
                    case "Спортсмен":
                        cmd.CommandText = "DELETE FROM Спортсмен WHERE [Номер спортсмена] = @Temp1";
                        break;
                    case "Команда":
                        cmd.CommandText = "DELETE FROM Команда WHERE [Идентификатор команды] = @Temp1";
                        break;
                    case "Результат":
                        cmd.CommandText = "DELETE FROM Результат WHERE [Номер попытки] = @Temp1";
                        break;
                    case "Стадион":
                        cmd.CommandText = "DELETE FROM Стадион WHERE [Идентификатор стадиона] = @Temp1";
                        break;
                    case "Вид спорта":
                        cmd.CommandText = "DELETE FROM [Вид спорта] WHERE [Идентификатор вида спорта] = @Temp1";
                        break;
                }
                cmd.Parameters.AddWithValue("@Templ", ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        /**************************** Add ****************************/
        public string AddTeamToDB(Team team)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO Команда VALUES (@CommandID, @CommandName, @City, @SportsmanCount, @Trainer)";
                cmd.Parameters.AddWithValue("@CommandID", team.TeamID);
                cmd.Parameters.AddWithValue("@CommandName", team.TeamName);
                cmd.Parameters.AddWithValue("@City", team.City);
                cmd.Parameters.AddWithValue("@SportsmanCount", team.SportsmanCount);
                cmd.Parameters.AddWithValue("@Trainer", team.Trainer);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string AddContestToDB(Contest contest)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO Соревнование VALUES (@ContestID, @SportID, @StadiumID, @BeginDate, @EndDate)";
                cmd.Parameters.AddWithValue("@ContestID", contest.ContestID);
                cmd.Parameters.AddWithValue("@SportID", contest.SportID);
                cmd.Parameters.AddWithValue("@StadiumID", contest.StadiumID);
                cmd.Parameters.AddWithValue("@BeginDate", contest.BeginDate);
                cmd.Parameters.AddWithValue("@EndDate", contest.EndDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string AddResultToDB(Result result)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO Результат VALUES (@AttemptNumber, @ContestID, @TeamID, @Date, @AttemptResult)";
                cmd.Parameters.AddWithValue("@AttemptNumber", result.AttemptNumber);
                cmd.Parameters.AddWithValue("@ContestID", result.ContestID);
                cmd.Parameters.AddWithValue("@TeamID", result.TeamID);
                cmd.Parameters.AddWithValue("@Date", result.Date);
                cmd.Parameters.AddWithValue("@AttemptResult", result.AttemptResult);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string AddSportsmanToDB(Sportsman sportsman)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO Спортсмен VALUES (@SportsmanNumber, @TeamID, @Surname, @Name, @Patronymic)";
                cmd.Parameters.AddWithValue("@SportsmanNumber", sportsman.SportsmanNumber);
                cmd.Parameters.AddWithValue("@TeamID", sportsman.TeamID);
                cmd.Parameters.AddWithValue("@Surname", sportsman.Surname);
                cmd.Parameters.AddWithValue("@Name", sportsman.Name);
                cmd.Parameters.AddWithValue("@Patronymic", sportsman.Patronymic);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string AddStadiumToDB(Stadium stadium)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO Стадион VALUES (@StadiumID, @StadiumName, @StadiumAdress, @StadiumCapacity)";
                cmd.Parameters.AddWithValue("@StadiumID", stadium.StadiumID);
                cmd.Parameters.AddWithValue("@StadiumName", stadium.StadiumName);
                cmd.Parameters.AddWithValue("@StadiumAdress", stadium.StadiumAddress);
                cmd.Parameters.AddWithValue("@StadiumCapacity", stadium.StadiumCapacity);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string AddSportToDB(Sport sport)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText =
                "INSERT INTO [Вид спорта] VALUES (@SportID, @SportName)";
                cmd.Parameters.AddWithValue("@SportID", sport.SportID);
                cmd.Parameters.AddWithValue("@SportName", sport.SportName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        /*************************** Update ***************************/
        public string UpdateTeamInDB(Team team)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE Команда SET
                                    [Название команды] = @TeamName,
                                    Город = @City,
                                    [Количество игроков] = @SportsmanCount,
                                    Тренер = @Trainer
                                    WHERE [Идентификатор команды] = @TeamID";

                cmd.Parameters.AddWithValue("@TeamName", team.TeamName);
                cmd.Parameters.AddWithValue("@City", team.City);
                cmd.Parameters.AddWithValue("@SportsmanCount", team.SportsmanCount);
                cmd.Parameters.AddWithValue("@Trainer", team.Trainer);
                cmd.Parameters.AddWithValue("@TeamID", team.TeamID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string UpdateContestInDB(Contest contest)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE Соревнование SET 
                                    [Идентификатор вида спорта] = @SportID,
                                    [Идентификатор стадиона] = @StadiumID,
                                    [Дата начала] = @BeginDate,
                                    [Дата окончания] = @EndDate
                                    WHERE [Идентификатор соревнования] = @ContestID";

                cmd.Parameters.AddWithValue("@SportID", contest.SportID);
                cmd.Parameters.AddWithValue("@StadiumID", contest.StadiumID);
                cmd.Parameters.AddWithValue("@BeginDate", contest.BeginDate);
                cmd.Parameters.AddWithValue("@EndDate", contest.EndDate);
                cmd.Parameters.AddWithValue("@ContestID", contest.ContestID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string UpdateResultInDB(Result result)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE Результат SET 
                                    [Идентификатор соревнования] = @ContestID,
                                    [Идентификатор команды] = @TeamID,
                                    [Дата выступления] = @Date,
                                    [Результат попытки] = @AttemptResult
                                    WHERE [Номер попытки] = @AttemptNumber";

                cmd.Parameters.AddWithValue("@ContestID", result.ContestID);
                cmd.Parameters.AddWithValue("@TeamID", result.TeamID);
                cmd.Parameters.AddWithValue("@Date", result.Date);
                cmd.Parameters.AddWithValue("@AttemptResult", result.AttemptResult);
                cmd.Parameters.AddWithValue("@AttemptNumber", result.AttemptNumber);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string UpdateSportsmanInDB(Sportsman sportsman)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE Спортсмен SET 
                                    [Идентификатор команды] = @TeamID,
                                    Фамилия = @Surname,
                                    Имя = @Name,
                                    Отчество = @Patronymic
                                    WHERE [Номер спортсмена] = @SportsmanNumber";

                cmd.Parameters.AddWithValue("@TeamID", sportsman.TeamID);
                cmd.Parameters.AddWithValue("@Surname", sportsman.Surname);
                cmd.Parameters.AddWithValue("@Name", sportsman.Name);
                cmd.Parameters.AddWithValue("@Patronymic", sportsman.Patronymic);
                cmd.Parameters.AddWithValue("@SportsmanNumber", sportsman.SportsmanNumber);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string UpdateStadiumInDB(Stadium stadium)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE Стадион SET 
                                    [Название стадиона] = @StadiumName,
                                    Адрес = @StadiumAdress,
                                    Вместимость = @StadiumCapacity
                                    WHERE [Идентификатор стадиона] = @StadiumID";

                cmd.Parameters.AddWithValue("@StadiumName", stadium.StadiumName);
                cmd.Parameters.AddWithValue("@StadiumAdress", stadium.StadiumAddress);
                cmd.Parameters.AddWithValue("@StadiumCapacity", stadium.StadiumCapacity);
                cmd.Parameters.AddWithValue("@StadiumID", stadium.StadiumID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        public string UpdateSportInDB(Sport sport)
        {
            string ex = null;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = @"UPDATE [Вид спорта] SET 
                                    [Название вида спорта] = @SportName
                                    WHERE [Идентификатор вида спорта] = @SportID";

                cmd.Parameters.AddWithValue("@SportName", sport.SportName);
                cmd.Parameters.AddWithValue("@SportID", sport.SportID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ex = e.Message;
            }
            finally
            {
                cn.Close();
            }
            return ex;
        }

        /*************************** Task 2 ***************************/
        public bool MSysObjectsRequest(string commandText)
        {
            bool flag = true;
            MSysObjectsData.Clear();
            MSysObjectsData = new DataTable();
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                //cmd.CommandText = "SELECT * FROM MSysObjects";
                //cmd.CommandText = "SELECT * FROM MSysACEs";
                //cmd.CommandText = "SELECT * FROM MSysQueries";
                //cmd.CommandText = "SELECT * FROM MSysRelationships";
                //cmd.CommandText = "SELECT * FROM MSysComplexColumns";
                //cmd.CommandText = "SELECT * FROM MSysAccessStorage";
                cmd.CommandText = commandText;
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        MSysObjectsData.Columns.Add(new DataColumn(rd.GetName(i)));
                    }
                    while (rd.Read())
                    {
                        string[] temp = new string[rd.FieldCount];
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            temp[i] = rd.GetValue(i).ToString();
                        }
                        MSysObjectsData.LoadDataRow(temp, false);
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        /*************************** Task 3 ***************************/
        public bool Request1()
        {
            results.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select * FROM [Все результаты от 10 до 15]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        results.Add(new Result(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetInt32(4)));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request2()
        {
            contests.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select * FROM [Все соревнования, начавшиеся после 6/10/2020]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        contests.Add(new Contest(rd.GetInt32(0), rd.GetInt32(1), rd.GetInt32(2),
                            rd.GetValue(3).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0],
                            rd.GetValue(4).ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request3()
        {
            teams.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "SELECT * FROM [Все команды из Химок и Зеленограда]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        teams.Add(new Team(rd.GetInt32(0), rd.GetString(1), !rd.IsDBNull(2) ? rd.GetString(2) : "",
                            rd.GetInt32(3), !rd.IsDBNull(4) ? rd.GetString(4) : ""));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request4(int ID)
        {
            sportsmans.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("Введите идентификатор команды", ID);
                cmd.CommandText = "SELECT * FROM [Все спортсмены конкретной команды]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        sportsmans.Add(new Sportsman(rd.GetInt32(0), rd.GetInt32(1),
                             !rd.IsDBNull(2) ? rd.GetString(2) : "", rd.GetString(3),
                             !rd.IsDBNull(4) ? rd.GetString(4) : ""));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request5(string city)
        {
            teams.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("Введите город", city);
                cmd.CommandText = "SELECT * FROM [Все команды конкретного города]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        teams.Add(new Team(rd.GetInt32(0), rd.GetString(1), !rd.IsDBNull(2) ? rd.GetString(2) : "",
                            rd.GetInt32(3), !rd.IsDBNull(4) ? rd.GetString(4) : ""));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request6(int capacity)
        {
            stadiums.Clear();
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("Введите вместимость", capacity);
                cmd.CommandText = "SELECT * FROM [Стадионы с вместимостью меньше конкретной]";
                OleDbDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        stadiums.Add(new Stadium(rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetInt32(3)));
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request7()
        {
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "EXEC [Добавить вид спорта с параметрами] 7, 'Охота на монстров'";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request8()
        {
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "EXEC [Удалить вид спорта с параметрами] 'Охота на монстров'";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }

        public bool Request9()
        {
            bool flag = true;
            cn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cn;
                cmd.CommandText = "EXEC [Заменить вид спорта на Гвинт с параметрами] 'Охота на монстров'";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                cn.Close();
            }
            return flag;
        }
    }
}
