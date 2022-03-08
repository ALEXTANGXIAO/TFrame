package MQ

import (
	"fmt"
	"log"

	"github.com/streadway/amqp"
)

//https://blog.csdn.net/qq_38151401/article/details/105688928

//url格式 amqp://账号:密码@rabbitmq服务器地址:端口号/vhost
const MQURL = "amqp://imoocuser:imoocuser@192.168.1.109:5672/imooc"

//const MQURL = "amqp://guest:guest@192.168.1.109:5672/"

type RabbitMQ struct {
	conn      *amqp.Connection
	channel   *amqp.Channel
	QueueName string //队列名称
	Exchange  string //交换机
	Key       string //Key
	Mqurl     string //连接信息
}

//创建RabbitMQ结构体实例
func NewRabbitMQ(queueName string, exchange string, key string) *RabbitMQ {
	rabbitMQ := &RabbitMQ{
		QueueName: queueName,
		Exchange:  exchange,
		Key:       key,
		Mqurl:     MQURL,
	}
	var err error
	//创建rabbitmq连接
	rabbitMQ.conn, err = amqp.Dial(rabbitMQ.Mqurl)
	rabbitMQ.failOnErr(err, "创建连接错误")
	rabbitMQ.channel, err = rabbitMQ.conn.Channel()
	rabbitMQ.failOnErr(err, "获取Channel失败")
	return rabbitMQ
}

//断开channel和connection
func (r *RabbitMQ) Destoryy() {
	r.channel.Close()
	r.conn.Close()
}

//错误处理函数
func (r *RabbitMQ) failOnErr(err error, message string) {
	if err != nil {
		log.Fatalf("%s:%s\n", message, err)
		panic(fmt.Sprintf("%s:%s", message, err))
	}
}

//简单模式Step：1、创建简单模式下RabbitMQ实例
func NewRabbitMQSimple(queueName string) *RabbitMQ {
	return NewRabbitMQ(queueName, "", "")
}

//简单模式Step：2、简单模式下生产代码
func (r *RabbitMQ) PublishSimple(message string) {
	//1、申请消息队列，如果队列不存在会自动创建，如果存在则跳过创建
	//好处：宝成队列存在，消息能发送到队列中
	_, err := r.channel.QueueDeclare(
		r.QueueName,
		//是否持久化
		false,
		//是否为自动删除
		false,
		//是否具有排他性
		false,
		//是否阻塞
		false,
		//额外属性
		nil,
	)
	if err != nil {
		fmt.Println(err)
	}
	//2、发送消息到队列中
	r.channel.Publish(
		r.Exchange,
		r.QueueName,
		//如果为true，根据exchange类型和routkey规则，如果无法找到符合条件的队列，那么会把发送的消息返回给发送者
		false,
		//如果为true，当exchange发送消息到队列侯发现队列上没有绑定消费者，则会把消息发还给发送者
		false,
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(message),
		})
}

//简单模式Step：3、消费
func (r *RabbitMQ) ConsumeSimple() {
	//1、申请队列
	_, err := r.channel.QueueDeclare(r.QueueName, false, false, false, false, nil)
	if err != nil {
		fmt.Println(err)
	}
	//2、接收消息
	msgs, err := r.channel.Consume(
		r.QueueName,
		//用来区分多个消费者
		"",
		//是否自动应答
		true,
		//是否具有排他性
		false,
		//如果设置为true，表示不能将同一个connection中发送的消息传递给这个connection中的消费者
		false,
		//消息队列是否阻塞
		false,
		nil,
	)
	if err != nil {
		fmt.Println(err)
	}

	forever := make(chan bool)

	//3、启动协程处理消息
	go func() {
		for d := range msgs {
			//实现我们要处理的逻辑函数
			log.Printf("Received a message : %s", d.Body)
		}
	}()

	log.Printf("[*] Waiting for messagees,To exit press CTRL+C")

	<-forever
}

//订阅模式Step：1、创建订阅模式下RabbitMQ实例
func NewRabbitMQPubSub(exchangeName string) *RabbitMQ {
	//创建RabbitMq实例
	return NewRabbitMQ("", exchangeName, "")
}

//订阅模式Step：2、订阅模式下生产代码
func (r *RabbitMQ) PublishgPub(message string) {
	//1、尝试创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		"fanout", //广播类型
		true,
		false,
		//true表示这个exchange不可以被client用来推送消息，仅用来进行exchange和exchange之间的绑定
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、发送消息
	err = r.channel.Publish(
		r.Exchange,
		"",
		false,
		false,
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(message),
		})
}

//订阅模式Step：3、订阅模式下消费代码
func (r *RabbitMQ) ReceiveSub() {
	//1、试探性创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		//交换机类型
		"fanout",
		true,
		false,
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、试探性创建队列，这里注意队列名称不要写
	q, err := r.channel.QueueDeclare(
		"", //随机生产队列名称
		false,
		false,
		true,
		false,
		nil,
	)
	r.failOnErr(err, "Failed to declare a queue")

	//3、绑定队列到exchange中
	err = r.channel.QueueBind(
		q.Name,
		//在Pub/Sub模式下，这里的key要为空
		"",
		r.Exchange,
		false,
		nil,
	)

	//4、消费信息
	message, err := r.channel.Consume(
		q.Name,
		"",
		true,
		false,
		false,
		false,
		nil,
	)

	forever := make(chan bool)

	//5、启动协程处理消息
	go func() {
		for d := range message {
			//实现我们要处理的逻辑函数
			log.Printf("Received a message : %s", d.Body)
		}
	}()

	log.Printf("[*] Waiting for messagees,To exit press CTRL+C")

	<-forever
}

//路由模式Step：1、创建路由模式下RabbitMQ实例
func NewRabbitMQRouting(exchangeName string, routingKey string) *RabbitMQ {
	//创建RabbitMq实例
	return NewRabbitMQ("", exchangeName, routingKey)
}

//路由模式Step：2、路由模式下生产代码
func (r *RabbitMQ) PublishgRouting(message string) {
	//1、尝试创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		"direct", //路由类型
		true,
		false,
		//true表示这个exchange不可以被client用来推送消息，仅用来进行exchange和exchange之间的绑定
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、发送消息
	err = r.channel.Publish(
		r.Exchange,
		r.Key,
		false,
		false,
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(message),
		})
}

//路由模式Step：3、路由模式下消费代码
func (r *RabbitMQ) ReceiveRouting() {
	//1、试探性创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		//交换机类型
		"direct",
		true,
		false,
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、试探性创建队列，这里注意队列名称不要写
	q, err := r.channel.QueueDeclare(
		"", //随机生产队列名称
		false,
		false,
		true,
		false,
		nil,
	)
	r.failOnErr(err, "Failed to declare a queue")

	//3、绑定队列到exchange中
	err = r.channel.QueueBind(
		q.Name,
		//在Pub/Sub模式下，这里的key要为空
		r.Key,
		r.Exchange,
		false,
		nil,
	)

	//4、消费信息
	message, err := r.channel.Consume(
		q.Name,
		"",
		true,
		false,
		false,
		false,
		nil,
	)

	forever := make(chan bool)

	//5、启动协程处理消息
	go func() {
		for d := range message {
			//实现我们要处理的逻辑函数
			log.Printf("Received a message : %s", d.Body)
		}
	}()

	log.Printf("[*] Waiting for messagees,To exit press CTRL+C")

	<-forever
}

//话题模式Step：1、创建话题模式下RabbitMQ实例
func NewRabbitMQTopic(exchangeName string, routingKey string) *RabbitMQ {
	//创建RabbitMq实例
	return NewRabbitMQ("", exchangeName, routingKey)
}

//话题模式Step：2、话题模式下生产代码
func (r *RabbitMQ) PublishgTopic(message string) {
	//1、尝试创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		"topic", //话题类型
		true,
		false,
		//true表示这个exchange不可以被client用来推送消息，仅用来进行exchange和exchange之间的绑定
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、发送消息
	err = r.channel.Publish(
		r.Exchange,
		r.Key,
		false,
		false,
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(message),
		})
}

//话题模式Step：3、话题模式下消费代码
//“*“匹配一个单词；"#"匹配多个单词（可以是0个）
//匹配imooc.*表示匹配imooc.hello，但是imooc.hello.one需要用到imooc.#才能匹配到
func (r *RabbitMQ) ReceiveTopic() {
	//1、试探性创建交换机
	err := r.channel.ExchangeDeclare(
		r.Exchange,
		//交换机类型
		"topic",
		true,
		false,
		false,
		false,
		nil,
	)
	r.failOnErr(err, "Failed t declare an exchange")

	//2、试探性创建队列，这里注意队列名称不要写
	q, err := r.channel.QueueDeclare(
		"", //随机生产队列名称
		false,
		false,
		true,
		false,
		nil,
	)
	r.failOnErr(err, "Failed to declare a queue")

	//3、绑定队列到exchange中
	err = r.channel.QueueBind(
		q.Name,
		//在Pub/Sub模式下，这里的key要为空
		r.Key,
		r.Exchange,
		false,
		nil,
	)

	//4、消费信息
	message, err := r.channel.Consume(
		q.Name,
		"",
		true,
		false,
		false,
		false,
		nil,
	)

	forever := make(chan bool)

	//5、启动协程处理消息
	go func() {
		for d := range message {
			//实现我们要处理的逻辑函数
			log.Printf("Received a message : %s", d.Body)
		}
	}()

	log.Printf("[*] Waiting for messagees,To exit press CTRL+C")

	<-forever
}
