# ShippingOrder API Documentation

## Огляд
API для управління замовленнями доставки, який підтримує повний CRUD функціонал та додаткові можливості.

## База URL
```
http://localhost:5051/api/ShippingOrder
```

## Авторизація
Всі endpoint'и вимагають авторизації через Bearer token в заголовку:
```
Authorization: Bearer <your-jwt-token>
```

## Endpoint'и

### 1. Отримати всі замовлення
- **GET** `/api/ShippingOrder`
- **Опис**: Отримати список всіх замовлень доставки
- **Відповідь**: 200 OK з масивом ShippingOrderDto

### 2. Отримати замовлення за ID
- **GET** `/api/ShippingOrder/{id}`
- **Параметри**: 
  - `id` (Guid) - ID замовлення
- **Відповідь**: 
  - 200 OK з ShippingOrderDto
  - 404 Not Found якщо замовлення не знайдено

### 3. Отримати замовлення клієнта
- **GET** `/api/ShippingOrder/customer/{customerId}`
- **Параметри**: 
  - `customerId` (Guid) - ID клієнта
- **Відповідь**: 200 OK з масивом ShippingOrderDto

### 4. Отримати детальні замовлення
- **GET** `/api/ShippingOrder/detailed?customerId={customerId}`
- **Query параметри**: 
  - `customerId` (Guid, опціонально) - ID клієнта для фільтрації
- **Опис**: Отримати замовлення з повними даними (включаючи пов'язані сутності)
- **Відповідь**: 200 OK з масивом ShippingOrderDto

### 5. Отримати замовлення з пагінацією
- **GET** `/api/ShippingOrder/paginated?pageNumber={pageNumber}&pageSize={pageSize}&customerId={customerId}`
- **Query параметри**: 
  - `pageNumber` (int, 1-∞) - Номер сторінки (за замовчуванням: 1)
  - `pageSize` (int, 1-100) - Розмір сторінки (за замовчуванням: 10)
  - `customerId` (Guid, опціонально) - ID клієнта для фільтрації
- **Відповідь**: 200 OK з масивом ShippingOrderDto

### 6. Створити замовлення
- **POST** `/api/ShippingOrder`
- **Body**: CreateShippingOrderDto
- **Відповідь**: 
  - 201 Created з ShippingOrderDto
  - 400 Bad Request якщо дані невірні

### 7. Оновити замовлення
- **PUT** `/api/ShippingOrder/{id}`
- **Параметри**: 
  - `id` (Guid) - ID замовлення
- **Body**: UpdateShippingOrderDto
- **Відповідь**: 
  - 204 No Content при успіху
  - 404 Not Found якщо замовлення не знайдено
  - 400 Bad Request якщо дані невірні

### 8. Видалити замовлення
- **DELETE** `/api/ShippingOrder/{id}`
- **Параметри**: 
  - `id` (Guid) - ID замовлення
- **Відповідь**: 
  - 204 No Content при успіху
  - 404 Not Found якщо замовлення не знайдено

## Моделі даних

### ShippingOrderDto
```json
{
  "id": "guid",
  "createdAt": "datetime",
  "lastUpdatedAt": "datetime",
  "customerId": "guid",
  "estimatedCost": "decimal",
  "estimatedDistance": "float",
  "estimatedShippingDate": "date",
  "estimatedShippingTime": "time"
}
```

### CreateShippingOrderDto
```json
{
  "customerId": "guid",
  "estimatedCost": "decimal",
  "estimatedDistance": "float",
  "estimatedShippingDate": "date",
  "estimatedShippingTime": "time"
}
```

### UpdateShippingOrderDto
```json
{
  "estimatedCost": "decimal (optional)",
  "estimatedDistance": "float (optional)",
  "estimatedShippingDate": "date (optional)",
  "estimatedShippingTime": "time (optional)"
}
```

## Приклади використання

### Створення замовлення
```bash
curl -X POST "http://localhost:5051/api/ShippingOrder" \
  -H "Authorization: Bearer <your-token>" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "123e4567-e89b-12d3-a456-426614174000",
    "estimatedCost": 150.50,
    "estimatedDistance": 25.5,
    "estimatedShippingDate": "2025-08-01",
    "estimatedShippingTime": "14:30:00"
  }'
```

### Отримання замовлень з пагінацією
```bash
curl -X GET "http://localhost:5051/api/ShippingOrder/paginated?pageNumber=1&pageSize=5" \
  -H "Authorization: Bearer <your-token>"
```

## Коди помилок

- **400 Bad Request**: Невірні дані запиту або порушення валідації
- **401 Unauthorized**: Відсутній або невірний токен авторизації
- **404 Not Found**: Ресурс не знайдено
- **500 Internal Server Error**: Внутрішня помилка сервера

## Валідація

- Всі GUID параметри мають бути валідними
- `pageNumber` має бути >= 1
- `pageSize` має бути від 1 до 100
- Всі обов'язкові поля в CreateShippingOrderDto мають бути заповнені
- `estimatedCost` має бути > 0
- `estimatedDistance` має бути > 0
