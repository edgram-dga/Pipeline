using Npgsql;
using MonitorInventario.Models;

public class DatabaseHelper
{
    public async Task<List<GroupMaterial>> GetGroupsAsync()
    {
        var list = new List<GroupMaterial>();
        await using var con = new NpgsqlConnection(AppConfig.ConnectionString);
        await con.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT gm.id, gm.name, array_agg(o.name) AS ops
            FROM grupo_material gm
            JOIN grupo_operaciones go ON gm.id = go.id_grupo
            JOIN operaciones o ON o.id = go.id_operacion
            GROUP BY gm.id, gm.name", con);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new GroupMaterial
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Operations = reader.GetFieldValue<string[]>(2).ToList()
            });
        }
        return list;
    }

    public async Task<DateTime?> GetLastRequestAsync(int groupId, string rangeTag)
    {
        await using var con = new NpgsqlConnection(AppConfig.ConnectionString);
        await con.OpenAsync();

        var cmd = new NpgsqlCommand("SELECT last_time FROM last_requested WHERE id_grupo=@g AND rango=@r", con);
        cmd.Parameters.AddWithValue("g", groupId);
        cmd.Parameters.AddWithValue("r", rangeTag);

        var result = await cmd.ExecuteScalarAsync();
        return result == null ? null : (DateTime?)result;
    }

    public async Task UpdateLastRequestAsync(int groupId, string rangeTag)
    {
        await using var con = new NpgsqlConnection(AppConfig.ConnectionString);
        await con.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            INSERT INTO last_requested(id_grupo, rango, last_time)
            VALUES(@g,@r,now())
            ON CONFLICT (id_grupo, rango) DO UPDATE SET last_time=now()", con);

        cmd.Parameters.AddWithValue("g", groupId);
        cmd.Parameters.AddWithValue("r", rangeTag);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpsertPieceAsync(PieceRecord p)
    {
        await using var con = new NpgsqlConnection(AppConfig.ConnectionString);
        await con.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            INSERT INTO piezas (id_pieza, item_number, item_type, item_version, item_description, fecha, operacion, status)
            VALUES(@id,@num,@type,@ver,@desc,@fecha,@op,@st)
            ON CONFLICT (id_pieza) DO UPDATE 
            SET operacion=@op, fecha=@fecha, status=@st", con);

        cmd.Parameters.AddWithValue("id", p.IdPieza);
        cmd.Parameters.AddWithValue("num", p.ItemNumber);
        cmd.Parameters.AddWithValue("type", p.ItemType);
        cmd.Parameters.AddWithValue("ver", p.ItemVersion);
        cmd.Parameters.AddWithValue("desc", p.ItemDescription);
        cmd.Parameters.AddWithValue("fecha", p.Date);
        cmd.Parameters.AddWithValue("op", p.Operation);
        cmd.Parameters.AddWithValue("st", p.Status);
        await cmd.ExecuteNonQueryAsync();
    }
}
