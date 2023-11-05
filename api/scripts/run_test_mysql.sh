cname="diagram-editor-mysql"
port="3306"
root_password="123456"

docker rm $cname --force > /dev/null

docker run \
    --name $cname \
    -e MYSQL_ROOT_PASSWORD=$root_password \
    -p $port:3306 \
    -d mysql

echo "mysql is initializing..."

while ! docker logs $cname 2>&1 | grep -m 1 -q "mysqld: ready for connections";
do
    sleep 1
done

docker exec -it $cname sh -c "mysql --version"

echo "running on localhost:$port"
echo "run 'mysql --password=$root_password' inside the container to enter mysql console"

