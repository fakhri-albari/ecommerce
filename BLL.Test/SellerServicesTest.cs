using API.DTO.Seller;
using DAL.Models;
using Moq;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Test
{
    public class SellerServicesTest
    {
        public class CreateSeller
        {
            public static List<object[]> GetSellerData => new List<object[]>
            {
                new Object[] 
                {
                    new Seller
                    {
                        name = "ucup",
                        email = "ucup@gmail.com",
                        password = "ucup123",
                        storeId = "ABC-123123",
                        store = new Store
                        {
                            name = "ABC Shop",
                            information = "Nothing"
                        }
                    }
                }
            };
                
            [Theory]
            [MemberData(nameof(GetSellerData))]
            public async Task CreateSeller_Succeed(Seller seller)
            {
                List<Seller> sellers = new List<Seller>();
                var mockRepo = new Mock<IDocumentDBRepository<Seller>>();
                mockRepo.Setup(c => c.CreateAsync(
                    It.IsAny<Seller>(),
                    It.IsAny<EventGridOptions>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((Seller p, EventGridOptions evg, string str1, string str2) => p)
                .Callback((Seller p, EventGridOptions evg, string createdBy, string activeFlag) =>
                {
                    sellers.Add(p);
                });

                var svc = new SellerServices(mockRepo.Object);
                var result = await svc.CreateSeller(seller);

                Assert.NotNull(result);
                Assert.NotEmpty(sellers);
                Assert.Equal(seller.name, sellers[0].name);
                Assert.Equal(seller.password, sellers[0].password);
                Assert.Equal(seller.email, sellers[0].email);
                Assert.Equal(seller.store, sellers[0].store);
            }
        }

        public class UpdateSeller
        {
            public static List<object[]> GetSellerData => new List<object[]>
            {
                new Object[]
                {
                    new Seller
                    {
                        Id = "ucup123",
                        name = "ucup",
                        email = "ucup@gmail.com",
                        password = "ucup123",
                        storeId = "ABC-123123",
                        store = new Store
                        {
                            name = "ABC Shop",
                            information = "Nothing"
                        }
                    },
                    new Seller
                    {
                        Id = "ucup123",
                        name = "budi",
                        email = "budi@gmail.com",
                        password = "budi123",
                        storeId = "ABC-123123",
                        store = new Store
                        {
                            name = "ABC Shop",
                            information = "Nothing"
                        }
                    }
                }
            };

            [Theory]
            [MemberData(nameof(GetSellerData))]
            public async Task UpdateSeller_Succeed(Seller oldData, Seller updatedData)
            {
 
                var mockRepo = new Mock<IDocumentDBRepository<Seller>>();

                mockRepo.Setup(c => c.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()))
                    .Returns(Task.FromResult(oldData));

                mockRepo.Setup(c => c.UpdateAsync(
                   It.IsAny<string>(),
                   It.IsAny<Seller>(),
                   It.IsAny<EventGridOptions>(),
                   It.IsAny<string>(),
                   It.IsAny<bool>()))
                    .ReturnsAsync((string id, Seller item, EventGridOptions options, string lastUpdatedBy, bool isOptimisticConcurrency) => item);

                var svc = new SellerServices(mockRepo.Object);
                var result = await svc.UpdateSeller(updatedData);

                Assert.Equal(result.name, updatedData.name);
                Assert.Equal(result.email, updatedData.email);
                Assert.Equal(result.password, updatedData.password);
            }

        }

        public class UpdateStore
        {
            public static List<object[]> GetSellerData => new List<object[]>
            {
                new Object[]
                {
                    new List<Seller>()
                    {
                        new Seller
                        {
                            Id = "ucup123",
                            name = "ucup",
                            email = "ucup@gmail.com",
                            password = "ucup123",
                            storeId = "ABC-12345",
                            store = new Store
                            {
                                name = "ABC Shop",
                                information = "Nothing"
                            }
                        },
                        new Seller
                        {
                            Id = "budi123",
                            name = "budi",
                            email = "budi@gmail.com",
                            password = "ucup123",
                            storeId = "ABC-12345",
                            store = new Store
                            {
                                name = "ABC Shop",
                                information = "Nothing"
                            }
                        }
                    },
                    new Seller
                    {
                        storeId = "ABC-12345",
                        store = new Store
                        {
                            name = "ABC Olshop",
                            information = "Blank"
                        }
                    }
                }
            };

            [Theory]
            [MemberData(nameof(GetSellerData))]
            public async Task UpdateStore_Succeed(List<Seller> sellers, Seller updatedStore)
            {
                var listResult = new List<Seller>();
                var mockRepo = new Mock<IDocumentDBRepository<Seller>>();
                mockRepo.Setup(c => c.GetAsync(
                    null,
                    It.IsAny<Func<IQueryable<Seller>, IOrderedQueryable<Seller>>>(),
                    It.IsAny<Expression<Func<Seller, Seller>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<string>()
                    )
                ).Returns(Task.FromResult(new PageResult<Seller>(sellers, "", diagnostic: "")));

                mockRepo.Setup(c => c.UpdateAsync(
                   It.IsAny<string>(),
                   It.IsAny<Seller>(),
                   It.IsAny<EventGridOptions>(),
                   It.IsAny<string>(),
                   It.IsAny<bool>()))
                    .ReturnsAsync((string id, Seller item, EventGridOptions options, string lastUpdatedBy, bool isOptimisticConcurrency) => item)
                    .Callback((string id, Seller item, EventGridOptions options, string lastUpdatedBy, bool isOptimisticConcurrency) =>
                    {
                        listResult.Add(item);
                    });

                var svc = new SellerServices(mockRepo.Object);

                var res = await svc.UpdateStore(updatedStore);

                //Assert.Equal(sellers.Count, listResult.Count);
                for(int i = 0; i < sellers.Count; i++)
                {
                    Assert.Equal(updatedStore.store, listResult[i].store);
                }
            }
        }
    }
}
